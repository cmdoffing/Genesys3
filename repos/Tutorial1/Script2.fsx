type Rainbow = { Boss    : string
                 Lackeys : string list }


let rainbow2 =
    { rainbow with Boss    = "Jeffrey"
                   Lackeys = [ "Zippy" ; "George" ; "Bungle" ]
    }