module Step.Views

open Giraffe.ViewEngine
open Step.Types
open Step.Models

let formCss = """
        form  { display: table;      }
        p     { display: table-row;  }
        label { display: table-cell; }
        input { display: table-cell; }
    """

let stepInputView (stepInput: StepInput) =
    div [] [
        form [_class formCss; _method "post"; _action ""] [
            p [] [
                label [_for "stepName"] [Text "Step Name: "]
                input [_id  "stepName"; _name "stepName"]
            ]

            p [] [
                label [_for "stepDoc"] [Text "Step Documentation: "]
                textarea [_id "stepDoc"; _name "stepDoc"; _rows "8"; _cols "40";
                          _placeholder "Describe this field using terms familiar to the user."] []
            ]
            button [_type "submit"] [Text "Save"]
        ]

        p [] [Text "Step input view"]
        p [] [str stepInput.StepInputName]
        p [] [str stepInput.StepInputDoc ]
    ]
