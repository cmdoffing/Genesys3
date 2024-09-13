module romanNumerals

open System.Text

let romanToArabic (roman : string) =
    let singleCharNumerals = roman.Replace("CM", "A")
                                  .Replace("CD", "B")
                                  .Replace("XC", "E")
                                  .Replace("XL", "F")
                                  .Replace("IX", "G")
                                  .Replace("IV", "H")

    let numeralsMap : Map<char, int> =
        [ ('A', 900); ('B', 400); ('E', 90); ('F', 40); ('G', 9); ('H', 4) ]
        |> Map.ofList

    singleCharNumerals
    |> Seq.map (fun c -> numeralsMap.Item c)
    |> Seq.sum


[<EntryPoint>] 
let main argv =
  let roman  = "MCMLXLIV"   // 1944
  let digits = romanToArabic roman

  printf ("Roman numerals are: %s") roman
  0