namespace Livingstone.Application

open System



module PublicTypes =

    type ApplicationName = private ApplicationName of string
    type FileStorage = private FileStorage of string
    type Version = private Version of string


    module ApplicationName =
        let value (ApplicationName str) = str

        let create  str = ApplicationName(str)
            
    module FileStorage =

        let value (FileStorage str) = str

        let create  str = FileStorage(str)

    module Version =

        let value (Version str) = str
        let create  str = Version(str)        


    

    type UnvalidatedBuildInfos = 
        {   
            version : Version
            helmValues : FileStorage
            data : FileStorage option
            applicationName : ApplicationName
        }       

    type ValidatedBuildInfos = {
        version : Version
        helmValues : FileStorage
        data : FileStorage option
        applicationName : ApplicationName
    }

    type NewApplicationCreated = {
        name: string
    }

    type Build = {
        version : Version
        helmValues : FileStorage
        data : FileStorage option
    }

    type BuildAddedToApplication = {
        name: string
        newBuild : string
    }

    type ApplicationEvent =
    | BuildAddedToApplication of BuildAddedToApplication

    type VersionError = {
        BadVersion : string
    }

    type ApplicationNotFoundError = {
        ApplicationNotfound : string
    }
    
    type ApplicationEnded = { 
        endDate: DateTime
    }

    type SaveError = {
        ex : Exception
    }

    type PublishError = {
        eventsPublished : ApplicationEvent list
        eventsNotPublished : ApplicationEvent list
        ex : Exception
    }

    type ValidationError = ValidationError of string


    type ApplicationError =
    | Validation of ValidationError
    | ApplicationNotFound of ApplicationNotFoundError
    | ApplicationEnded of ApplicationEnded
    | PublishError of PublishError
    | SaveError of SaveError


    type NewApplication =
        {
            name : string
            port: int
        }

    type KnownApplication =
        {
            name : string
            port: int
            builds : Build list
        }       

    type Application =
    | NewApplication of NewApplication
    | KnownApplication of KnownApplication
    


    type AddBuild =
        UnvalidatedBuildInfos -> AsyncResult<Application*ApplicationEvent list, ApplicationError>


module internal InternalTypes =

    open PublicTypes


    type GetApplication = ApplicationName -> AsyncResult<KnownApplication, ApplicationNotFoundError> 

    type CheckApplicationExists = string -> bool

    type PublishEvents =
        ApplicationEvent list -> AsyncResult<unit, PublishError>    


    type SaveApplication =
        Application -> AsyncResult<unit, SaveError>    


    type ValidateBuildInfos =
          CheckApplicationExists -> UnvalidatedBuildInfos -> AsyncResult<ValidatedBuildInfos, ValidationError>    

    type AddBuildInfos =
        GetApplication -> ValidatedBuildInfos ->  AsyncResult<Application, ApplicationNotFoundError>

    

    type CreateEvents = 
        Application                           // input
         -> ApplicationEvent list              // output     
            