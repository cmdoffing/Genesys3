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
        table [ _class "table table-hover" ] [
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
        form [_method "post"; _action domainInsertUrl] [
            fieldset [] [
                legend [] [Text "Create New Domain"]

                input [_hidden; _name "DomainId" ;     _value "1"]        // Fix. Set to Guid
                input [_hidden; _name "ContextId";     _value (string Context.curContextId) ]
                input [_hidden; _name "DomainDeleted"; _value "false"]    // Fix if necessary


                div [] [
                    label [_for "ContextName"] [Text "Context Name: "]
                    input [_type "text"; _id "ContextName"; _name "ContextName"; _size "40"]
                ]
                div [] [
                    label [_for "DomainName"] [Text "Domain Name: "]
                    input [_type "text"; _id "DomainName"; _name "DomainName";
                           _size "40"; _required]
                ]
                div [] [
                    label [_for "DomainDoc"] [Text "Domain Documentation: "]
                    textarea [_id "DomainDoc"; _name "DomainDoc"; _maxlength maxDocLength;
                              _rows numDocTextAreaRows; _cols numDocTextAreaCols]
                             []
                ]
                button [_type "submit"] [Text "Save"]
            ]
        ]
    ]


let domainEditView (domain: Domain) =
    div [] [
        form [_method "post"; _action domainUpdateUrl] [
            fieldset [] [
                legend [] [Text "Domain Detail"]

                input [_type "hidden"; _name "DomainId";  _value (string domain.DomainId)  ]
                input [_type "hidden"; _name "ContextId"; _value (string domain.ContextId) ]

                div [] [
                    label [_for "ContextName"] [Text "Context Name: "]
                    br []
                    input [_type "text"; _id "ContextName"; _name "ContextName";
                           _value domain.ContextName; _size "40"]
                ]
                div [] [
                    label [_for "DomainName"] [Text "Domain Name: "]
                    input [_type "text"; _id "DomainName"; _name "DomainName";
                           _value domain.DomainName; _size "40"; _required ]
                ]
                div [] [
                    label [_for "DomainDoc"] [Text "Domain Documentation: "]
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

let domainsPage = MasterViews.documentView domainsView

//-----------------------------------------------------------------------

let domainGetEndpoints =
    GET [
        route  "/domain"  (text Database.domainString)
        route  "/domains" (htmlView domainsPage)
    ]
