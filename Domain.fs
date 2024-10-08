module Domain

open Giraffe

[<CLIMutable>]
type Domain = {
    DomainId      : int64
    ContextId     : int64
    ContextName   : string
    DomainName    : string
    DomainDoc     : string
    DomainDeleted : bool
}

//---------------------------------------------------------------------
//                     Database and Data Access
//---------------------------------------------------------------------

exception DomainDbError of string

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

let tryGetDomain domainId =
    List.tryFind (fun d -> domainId = d.DomainId) domains

let getDomain domainId =
        match tryGetDomain domainId with
        | Some d -> d
        | None   -> let msg = sprintf "Domain not found. domainId = %d" domainId
                    raise (Database.DatabaseError msg)

let private insertDomainIntoDb domain =
    ignore    // Fix

let updateDomain domain =
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

let domainRows theDomains =
    List.map (fun d -> domainRow d) theDomains

let domainListview domainListRows =
    div [] [
        table [ _class "table table-hover table-bordered" ] [
            thead [] [
                tr [] [
                    th [ _scope "col" ] [Text "Context" ]
                    th [ _scope "col" ] [Text "Domain Name" ]
                    th [ _scope "col" ] [Text "Documentation" ]
                ]
            ]
            p [] domainListRows
        ]
    ]

let maxDocLength       = "2000"     // Must be a string to be used as an HTML attribute value
let numDocTextAreaRows = "12"
let numDocTextAreaCols = "65"
 
let domainNewView =
    div [] [
        form [_method "post"; _class "form-horizontal"; _action domainInsertUrl] [
            div [_class "form-group"] [
                legend [] [Text "Create New Domain"]

                input [_hidden; _name "DomainId" ; _value "1"]        // Fix. Set to Guid
                input [_hidden; _name "ContextId";
                       _value (string Context.curContextId)]
                input [_hidden; _name "DomainDeleted"]    // Fix if necessary

                div [] [
                    label [_class "control-label col-sm-2"; _for "ContextName"]
                          [Text "Context Name"]
                    div [_class "col-sm-10"] [
                        input [_type "text"; _class "form-control"; _id "ContextName";
                               _name "ContextName"; _size "40"; _required]
                    ]
                ]
                div [] [
                    label [_class "control-label col-sm-2"; _for "DomainName"]
                          [Text "Domain Name"]
                    div [_class "col-sm-10"] [
                        input [_type "text"; _id "DomainName"; _name "DomainName";
                               _size "40"; _required]
                    ]
                ]
                div [] [
                    label [_class "control-label col-sm-2"; _for "DomainDoc"]
                          [Text "Domain Documentation"]
                    div [_class "col-sm-10"] [
                        textarea [_id "DomainDoc"; _name "DomainDoc";
                                  _maxlength maxDocLength;
                                  // _rows numDocTextAreaRows; _cols numDocTextAreaCols
                                  ] 
                                  []
                    ]
                ]
                div [_class "btn-group"] [
                    button [_type "submit"; _class "btn btn-primary"] [Text "Save"]
                    span [] [Text " "]
                    button [_type "button"; _class "btn btn-default"] [Text "Cancel"]
                ]
            ]
        ]
    ]


let domainEditView (domain: Domain) =
    div [_class ""] [
        form [_method "post"; _action domainUpdateUrl] [
            fieldset [] [
                legend [] [Text "Domain Detail"]

                input [_type "hidden"; _name "DomainId";  _value (string domain.DomainId)  ]
                input [_type "hidden"; _name "ContextId"; _value (string domain.ContextId) ]

                div [_class "mb-3"] [
                    label [_for "ContextName"] [Text "Context Name"]
                    br []
                    input [_type "text"; _id "ContextName"; _name "ContextName";
                           _value domain.ContextName; _size "40"]
                ]
                div [_class "mb-3"] [
                    label [_for "DomainName"] [Text "Domain Name"]
                    br []
                    input [_type "text"; _id "DomainName"; _name "DomainName";
                           _value domain.DomainName; _size "40"; _required ]
                ]
                div [_class "mb-3"] [
                    label [_for "DomainDoc"] [Text "Domain Documentation"]
                    br []
                    textarea [_id "DomainDoc"; _name "DomainDoc"; _maxlength maxDocLength;
                              _rows numDocTextAreaRows; _cols numDocTextAreaCols]
                             [ str domain.DomainDoc ]
                ]
            ]
        ]
    ]

let domainIndexView =
    div [] [
        domainListview (domainRows domains)
        domainEditView domains.Head          // Fix
    ]

let domainIndexPage = documentView domainIndexView
let domainNewPage   = documentView domainNewView

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
            let showDomainPage = documentView (domainEditView domain)
            return! ctx.WriteHtmlViewAsync showDomainPage
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
