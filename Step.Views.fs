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
