module Livingstone.Deployment.Product.Rest

open Fable.PowerPack.Fetch

open Thoth.Json
open System


open Livingstone.Shared
open Livingstone.RestHelpers
open Livingstone.Deployment.Product.Types

let initialProduct = fetchAs<ProductVersionReadModel []> "/api/deployment/getProducts" (Decode.Auto.generateDecoder())

let deployProduct = post<DeployProductCmd,Guid> "/api/deployment/deployProduct" (Decode.Auto.generateDecoder())