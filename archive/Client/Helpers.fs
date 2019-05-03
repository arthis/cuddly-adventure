module Livingstone.ClientHelpers

open Fulma
open Fable.Import.React
open Elmish
open Elmish.React

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.PowerPack.Fetch

open Thoth.Json

open Livingstone.Shared


open Fulma
open System.Collections.Generic

 
let button2 txt onClick =
    Button.button
        [ 
          // Button.Color IsPrimary
          Button.OnClick (fun evt ->
            onClick evt
            evt.stopPropagation()
          )
        ]
        [ str txt ]

let button txt onClick =
    Button.button
        [ Button.IsFullWidth
          // Button.Color IsPrimary
          Button.OnClick onClick ]
        [ str txt ]


let columsOption a (reactElements: Fable.Import.React.ReactElement option list) =
  reactElements
  |> List.filter (fun c -> c.IsSome )
  |> List.map Option.get
  |> Columns.columns a

let dropdown<'a,'b> (isActive:'a->bool) currentValue (cmd:'a->'b) (elts:'a[]) (showValue:'a->string)  dispatch=
  Dropdown.dropdown [ Dropdown.IsHoverable ]
      [ div [ ]
          [ Button.button [ ]
              [ span [ ]
                  [ str currentValue ]
                Icon.icon [ Icon.Size IsSmall ]
                  [ ] ] ]
        Dropdown.menu [ ]
          [ Dropdown.content [ ]  (
              elts
              |> Seq.toList
              |> List.map (fun elt -> 
              Dropdown.Item.a [ 
                      Dropdown.Item.Props [ OnClick (fun evt -> 
                            dispatch <| cmd elt  
                          ) 
                        ]
                      Dropdown.Item.IsActive <| isActive  elt;
                  ] [ str <| showValue elt ]
              ) 
          )]
          
      ] 

let panelOptions switch f g =
  match switch with
  | Some(a) -> f a
  | None -> g
let formOptions a  (reactElements: (string*ReactElement) option list)=
    
  reactElements  
  |> List.filter (fun c -> c.IsSome )
  |> List.map (Option.get >> (fun (title,elt) -> 
    Field.div [] [
      Label.label [ ] [ str title ]
      Control.div [] [elt]
    ]))
  |> Content.content a

    
  
      

