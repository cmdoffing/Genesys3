module Domain

open Giraffe
open Giraffe.ViewEngine
open Giraffe.EndpointRouting

(*
open Database

let [<Literal>] selectDomainSql = "SELECT * FROM Domain WHERE DomainId = @domainId"

type DomainPT = SqlCommandProvider< selectDomainSql, connString>

let domainPT = new DomainPT( connString )
let domain   = domainPT.Execute( 1L ) |> Seq.head
let domainString = sprintf "%A" domain
*)

type Domain = {
    DomainId      : int64
    ContextId     : int64
    DomainName    : string
    DomainDoc     : string
    DomainDeleted : bool
}

let private domains = [
    {
        DomainId      = 1L
        ContextId     = 1L
        DomainName    = "Order fulfillment"
        DomainDoc     = "Order fulfillment docs"
        DomainDeleted = false
    }
    {
        DomainId      = 2L
        ContextId     = 1L
        DomainName    = "Billing"
        DomainDoc     = "Billing docs"
        DomainDeleted = false
    }
]

//---------------------------------------------------------------------
//                              Views
//---------------------------------------------------------------------

let domainRow domain =
    tr [] [
        td [] [str (string domain.ContextId)]
        td [] [str domain.DomainName]
        td [] [str domain.DomainDoc]
    ]

let domainRows domains =
    List.map (fun d -> domainRow d) domains

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

let domainView =
    div [] [
        //p [] [ Text "Domain Id: "; Text (string theDomain.DomainId)
        domainListview (domainRows domains)
    ]

let domainsPage = MasterViews.documentView domainsView

//-----------------------------------------------------------------------

let domainGetEndpoints =
    GET [
        route  "/domain"  (text Database.domainString)
        route  "/domains" (htmlView domainsPage)
    ]
