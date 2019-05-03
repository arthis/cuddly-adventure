namespace Livingstone.Application

open System

open Livingstone.Shared
open Livingstone.Application.PublicTypes

[<AutoOpen>]
module ApplicationDto =

    //COMMANDS
    [<CLIMutable>]
    type BuildInfosDto =
        {
            Version : string
            HelmValues :string
            Data : string
            ApplicationName : string
        }


    let toUnvalidatedBuildInfos (dto:BuildInfosDto) : UnvalidatedBuildInfos =

        let data =
            if (String.IsNullOrEmpty(dto.Data)) then None
            else Some <| FileStorage.create dto.Data

        // sometimes it's helpful to use an explicit type annotation 
        // to avoid ambiguity between records with the same field names.
        let domainObj : UnvalidatedBuildInfos = {  

            // this is a simple 1:1 copy which always succeeds
                version = Version.create dto.Version
                helmValues = FileStorage.create dto.HelmValues
                data = data
                applicationName = ApplicationName.create dto.ApplicationName
            } 
        domainObj 

