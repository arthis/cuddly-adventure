module Livingstone.RestHelpers

open System.Collections.Generic

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.PowerPack.Fetch
open Fable.Core.JsInterop
open Fable.PowerPack
open Fable.Import.React
open Elmish
open Elmish.React
open Fulma

open Thoth.Json

open Livingstone.Shared

let private errorString (response: Response) =
    string response.Status + " " + response.StatusText + " for URL " + response.Url

let post<'a,'b>  (url: string) (decoder: Decode.Decoder<'b>) (record:'a)  (init: RequestProperties list) =
    let defaultProps =
        [ RequestProperties.Method HttpMethod.POST
          requestHeaders [ContentType "application/json"]
          RequestProperties.Body !^(Encode.Auto.toString(0, record))]
    
    List.append defaultProps init
    |> fetch url
    |> Promise.bind (fun response ->
        if not response.Ok
        then errorString response |> failwith
        else 
            response.text() 
            |> Promise.map (fun res ->
                match Decode.fromString decoder res with
                | Ok successValue -> successValue
                | Error error -> failwith error))
