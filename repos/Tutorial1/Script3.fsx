open System

let decimalToRoman (num: int) : string =
    let romanNumerals = [| (1000, "M"); (900, "CM"); (500, "D"); (400, "CD");
                           (100, "C"); (90, "XC"); (50, "L"); (40, "XL");
                           (10, "X"); (9, "IX"); (5, "V"); (4, "IV");
                           (1, "I") |]
    
    let rec convert n (acc: string) =
        match n with
        | 0 -> acc
        | _ ->
            let (value, symbol) = Array.tryFind (fun (v, _) -> v <= n) romanNumerals |> Option.defaultValue (0, "")
            convert (n - value) (acc + symbol)

    convert num ""

let stringToRoman (input: string) : string =
    match Int32.TryParse(input) with
    | (true, num) -> decimalToRoman num
    | _ -> "Invalid input"

// Example usage
let roman = stringToRoman "1987"
printfn "%s" roman
