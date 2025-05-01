module Domain

open Giraffe

[<CLIMutable>]
type Domain = {
    DomainId      : int64   // Fix by setting to Guid
    ContextId     : int64   // Fix by setting to Guid
    ContextName   : string
    DomainName    : string
    DomainDoc     : string
    DomainDeleted : bool
}

//---------------------------------------------------------------------
//                     Database and Data Access
//---------------------------------------------------------------------

exception private DomainDbError of string

(*
open Database

let [<Literal>] selectDomainSql = "SELECT * FROM Domain WHERE DomainId = @domainId"

type DomainPT = SqlCommandProvider< selectDomainSql, connString>

let domainPT = new DomainPT( connString )
let domain   = domainPT.Execute( 1L ) |> Seq.head
let domainString = sprintf "%A" domain
*)

let private domains = [
    {
        DomainId      = 1L
        ContextId     = 1L
        ContextName   = "Orders"
        DomainName    = "Order fulfillment"
        DomainDoc     = "Order fulfillment docs"
        DomainDeleted = false
    }
    {
        DomainId      = 2L
        ContextId     = 1L
        ContextName   = "Orders"
        DomainName    = "Billing"
        DomainDoc     = "Billing docs"
        DomainDeleted = false
    }
]

let private tryGetDomain domainId =
    List.tryFind (fun d -> domainId = d.DomainId) domains

let private getDomain domainId =
        match tryGetDomain domainId with
        | Some d -> d
        | None   -> let msg = sprintf "Domain not found. domainId = %d" domainId
                    raise (Database.DatabaseError msg)

let private insertDomainIntoDb domain =
    ignore    // Fix

let private updateDomain domain =
    ignore

//---------------------------------------------------------------------
//                              Views
//---------------------------------------------------------------------
open Giraffe.ViewEngine
open MasterViews
open Urls

let private domainRow domain =
    tr [] [
        td [] [str domain.ContextName]
        td [] [str domain.DomainName]
        td [] [str domain.DomainDoc]
    ]

let private domainRows theDomains =
    List.map (fun d -> domainRow d) theDomains

let private domainListview domainListRows =
    div [] [
        table [_class "table table-hover table-bordered"] [
            thead [] [
                tr [] [
                    th [_scope "col" ] [Text "Context"]
                    th [_scope "col" ] [Text "Domain Name"]
                    th [_scope "col" ] [Text "Documentation"]
                ]
            ]
            p [] domainListRows
        ]
    ]

// These must be strings to be used as an HTML attribute value
let maxDocLength       = "2000"
let numDocTextAreaRows =    "6"
let numDocTextAreaCols =   "65"

let private domainNewView =
    div [] [
        form [_method "post"; _action domainInsertUrl] [
                legend []  [Text "Create New Domain"]
                input [_hidden; _name "DomainId" ; _value "1"]   // Fix. Set to Guid
                input [_hidden; _name "ContextId";
                       _value (string Context.curContextId)]
                input [_hidden; _name "DomainDeleted"]    // Fix if necessary

                div [_class "mb-3 mt-3"] [
                    label [_class "form-label"; _for "ContextName"]  [Text "Context Name"]
                    input [_type "text"; _class "form-control"; _id "ContextName";
                           _name "ContextName"; _size "40"; _required]
                ]
                div [_class "mb-3 mt-3"] [
                    label [_class "form-label"; _for "DomainName"]  [Text "Domain Name"]
                    input [_type "text"; _class "form-control"; _id "DomainName";
                           _size "40"; _name "DomainName"; _required]
                ]
                div [_class "mb-3 mt-3"] [
                    label [_class "form-label"; _for "DomainDoc"]  [Text "Domain Documentation"]
                    textarea [_id "DomainDoc"; _name "DomainDoc"; _maxlength maxDocLength;
                              _class "form-control";
                              _rows numDocTextAreaRows; _cols numDocTextAreaCols
                    ] []
                ]
                button [_type "submit"; _class "btn btn-primary"]  [Text "Save"]
                span   [] [Text " "]
                button [_type "button"; _class "btn btn-default"]  [Text "Cancel"]
        ]
    ]


let private domainEditView (domain: Domain) =
    div [] [
        form [_method "post"; _action domainUpdateUrl] [
            legend [] [Text "Domain Detail"]
            input  [_type "hidden"; _name "DomainId";  _value (string domain.DomainId)  ]
            input  [_type "hidden"; _name "ContextId"; _value (string domain.ContextId) ]

            div [_class "mb-3 mt-3"] [
                label [_for "ContextName"; _class "form-label"]  [Text "Context Name"]
                input [_type "text"; _id "ContextName"; _name "ContextName";
                        _value domain.ContextName; _size "40"; _class "form-control"]
            ]
            div [_class "mb-3 mt-3"] [
                label [_class "form-label"; _for "DomainName"]  [Text "Domain Name"]
                input [_type "text"; _id "DomainName"; _name "DomainName";
                       _value domain.DomainName; _size "40"; _class "form-control"; _required ]
            ]
            div [_class "mb-3"] [
                label [_class "form-label"; _for "DomainDoc"]  [Text "Domain Documentation"]
                textarea [_class "form-control"; _id "DomainDoc"; _name "DomainDoc";
                          _maxlength maxDocLength; _rows numDocTextAreaRows]  [str domain.DomainDoc]
            ]
            button [_type "submit"; _class "btn btn-primary"]  [Text "Save"]
            span [] [Text " "]
            button [_type "button"; _class "btn btn-secondary"]  [Text "Cancel"]
        ]
    ]

let private domainIndexView =
    div [] [
        domainListview (domainRows domains)
        br []
        hr []
        domainEditView domains.Head
    ]

let private domainIndexPage = documentView domainIndexView
let private domainNewPage   = documentView domainNewView

//---------------------------------------------------------------------
//                             Handlers
//---------------------------------------------------------------------
open Microsoft.AspNetCore.Http
open Giraffe.EndpointRouting

let private domainInsertHandler : HttpHandler =
    fun (next : HttpFunc) (ctx : HttpContext) ->
        task {
            let! domain = ctx.BindModelAsync<Domain>()
            insertDomainIntoDb domain |> ignore
            //return! Successful.OK (domainEditView domain) next ctx   // Sends the object back to the client
            let domainDisplayPage = documentView (domainEditView domain)
            return! ctx.WriteHtmlViewAsync domainDisplayPage
        }

let private domainDeleteHandler domainId =
    let domain        = getDomain domainId
    let deletedDomain = { domain with DomainDeleted = true }
    updateDomain deletedDomain |> ignore
    redirectTo false domainIndexUrl

//---------------------------------------------------------------------
//                             Routing
//---------------------------------------------------------------------
let domainGetEndpoints =
    GET [
        route domainDefaultUrl (redirectTo true domainIndexUrl)
        route domainIndexUrl   (htmlView domainIndexPage)
        route domainNewUrl     (htmlView domainNewPage)
    ]

let domainPostEndpoints =
    POST [
        route  domainInsertUrl     domainInsertHandler
        routef "/domain/delete/%d" domainDeleteHandler
    ]
