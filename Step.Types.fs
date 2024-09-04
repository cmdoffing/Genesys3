module Step.Types

//type Name = Name of string

//type Doc = Doc of string

[<CLIMutable>]
type StepInput = {
    StepInputName : string
    StepInputDoc  : string
}

type StepOutput = {
    StepOutputName : string
    StepOutputDoc  : string
}

type Transform = Transform of string

type CodeTemplate = CodeTemplate of string

type Route = Route of string

type HttpHeaderFunction = HeaderFunctionHeader of string

type FieldType = FieldType of string
(*
    | ShortText
    | LongText
    | Integer
    | Decimal
    | CalculatedField
    | Lookup
    | Date
    | DateTime
*)

type Validation = Validation of string

type DefaultValue = DefaultValue of string

type Field = {
    FieldName    : string
    FieldDoc     : string
    FieldType    : FieldType
    Validations  : Validation list
    Regex        : System.Text.RegularExpressions.Regex option
    DefaultValue : DefaultValue
}

type Step = {
    StepName      : string
    StepDoc       : string
    StepInputs    : StepInput list
    Output        : StepOutput
    Transforms    : Transform list
    Route         : Route  option
    HttpHeaderFunction: HttpHeaderFunction option
    ViewTemplate  : string option
    CodeTemplates : CodeTemplate list
    Fields        : Field list
    Validation    : Validation
}

type Workflow = {
    WorkflowName : string
    WorkflowDoc  : string
    Steps        : Step list
}

