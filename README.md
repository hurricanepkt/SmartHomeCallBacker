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
        volumes:
            - /AppData/SmartHomeCallBacker:/Data
```

## Source Code

[github.com/hurricanepkt/SmartHomeCallBacker](https://github.com/hurricanepkt/SmartHomeCallBacker)


## Contributing

Pull requests are welcome. For major changes, please open an issue first
to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License

[MIT](https://github.com/hurricanepkt/SmartHomeCallBacker/blob/main/LICENSE)