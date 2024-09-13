module Step.Types

type Name = Name of string

type Doc = Doc of string

type Input = {
    Name : Name
    Doc  : Doc
}

type Inputs = Inputs of Input list

type Output = {
    Name : Name
    Doc  : Doc
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
    Name         : Name
    Doc          : Doc
    FieldType    : FieldType
    Validations  : Validation list
    Regex        : System.Text.RegularExpressions.Regex option
    DefaultValue : DefaultValue
}

type Step = {
    Name          : Name
    Doc           : Doc
    Inputs        : Input list
    Output        : Output
    Transforms    : Transform list
    Route         : Route  option
    HttpHeaderFunction: HttpHeaderFunction option
    ViewTemplate  : string option
    CodeTemplates : CodeTemplate list
    Fields        : Field list
    Validation    : Validation
}

type Workflow = {
    Name : Name
    Doc  : Doc
    Steps: Step list
}