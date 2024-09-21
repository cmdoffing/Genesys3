module Domain

open Giraffe.ViewEngine

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
    ContextName   : string
    DomainName    : string
    DomainDoc     : string
    DomainDeleted : bool
}

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

//---------------------------------------------------------------------
//                              Views
//---------------------------------------------------------------------

let private domainRow theDomain =
    tr [] [
        td [] [str theDomain.ContextName]
        td [] [str theDomain.DomainName]
        td [] [str theDomain.DomainDoc]
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


(*
let stepInputView (stepInput: StepInput) =
    div [] [
        form [_class formCss; _method "post"; _action "/submitStepInput"] [
            p [] [
                label [_for "StepInputName"] [Text "Step Name: "]
                input [_id  "StepInputName"; _name "StepInputName"]
            ]

            p [] [
                label [_for "StepInputDoc"] [Text "Step Documentation: "]
                textarea [_id "StepInputDoc"; _name "StepInputDoc"; _rows "8"; _cols "40";
                          _placeholder "Describe this field using terms familiar to the user."] []
            ]
            button [_type "submit"] [Text "Save"]
        ]

        p [] [Text "Step input view"]
        p [] [str stepInput.StepInputName]
        p [] [str stepInput.StepInputDoc ]
    ]
*)


let getDomain domainId =
    List.find (fun d -> d.DomainId = domainId) domains

let domainSaveUrl = "/domain/save"

let maxDocLength       = "2000"     // Must be a string to work with attributes
let numDocTextAreaRows = "12"
let numDocTextAreaCols = "65"

let domainDetailView domainId =
    let theDomain = getDomain domainId

    div [] [
        form [_method "post"; _action domainSaveUrl] [
            fieldset [_disabled] [
                legend [] [Text "Domain Detail"]

                input [_type "hidden"; _value (string domainId); _name "DomainId"]
                div [] [
                    label [_for "ctxName"] [Text "Context Name: "]
                    input [_type "text"; _id "ctxName"; _value (string theDomain.ContextName)]
                ]
                div [] [
                    label [_for "domainName"] [Text "Domain Name: "]
                    input [_type "text"; _id "domainName"; _value theDomain.DomainName]
                ]
                div [] [
                    label [_for "domainDoc"] [Text "Domain Documentation: "]
                    textarea [_id "domainDoc"; _maxlength maxDocLength;
                              _rows numDocTextAreaRows;_cols numDocTextAreaCols] [
                        str theDomain.DomainDoc
                    ]
                ]
            ]
        ]
    ]

let domainsView =
    div [] [
        domainListview (domainRows domains)
        domainDetailView 1L
    ]

let domainsPage = MasterViews.documentView domainsView
