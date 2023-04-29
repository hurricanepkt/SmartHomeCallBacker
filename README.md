# SmartHomeCallBacker

Simple API exposed service that calls URLs (particularly webhooks) based on API calls

## Installation

Using docker compose 

```docker-compose.yml
    SmartHomeCallBacker:
        container_name: SmartHomeCallBacker
        image: markgreenway/smarthomecallbacker:latest
        restart: always
        ports: 
            - 8083:80
        environment: 
            CustomString : "EnvironmentVariablesSetCorrectly"
            MaxFailures : 15,
            CleanupAggressiveness : "AllComplete"
            ServiceFrequency: 5
            DataBaseType:  FileSystem
        volumes:
            - /AppData/SmartHomeCallBacker:/Data
```
## Getting Started 

You can see the getting started guide here : 
[github.com/hurricanepkt/SmartHomeCallBacker/wiki/Getting-Started](https://github.com/hurricanepkt/SmartHomeCallBacker/wiki/Getting-Started)

## Database Types

DatabaseType has several options FileSystem, SqlLiteInMemory, SqlLite

- FileSystem -->  writes human readable Json to /Data/json
- SqlLiteInMemory --> uses an in memory SQL lite db (very transient, clears when re-launching)
- SqlLite --> writes to file system /Data/sqlite/callbacks.db

## Source Code

[github.com/hurricanepkt/SmartHomeCallBacker](https://github.com/hurricanepkt/SmartHomeCallBacker)




## Contributing

Pull requests are welcome. For major changes, please open an issue first
to discuss what you would like to change.

Please make sure to update tests as appropriate.

## Thanks

[Ivan Stoev](https://stackoverflow.com/users/5202563/ivan-stoev) - For help on answer [StackOverflow](https://stackoverflow.com/questions/76082155/ef-context-with-flexibility-onconfiguring-throws-errors)

## License

[MIT](https://github.com/hurricanepkt/SmartHomeCallBacker/blob/main/LICENSE)

