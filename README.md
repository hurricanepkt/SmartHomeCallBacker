# SmartHomeCallBacker

Simple API exposed service that calls URLs (particularly webhooks) based on API calls

## Installation

Using docker compose 

```docker-compose.yml
    AISmarthome:
        container_name : AISmarthome
        image: markgreenway/aismarthome:latest
        ports:
            - 8082:80
            - "10111:10110/udp"
        restart: always
        volumes:
            - /AppData/aismarthome:/Data
```

## Source Code

[github.com/hurricanepkt/SmartHomeCallBacker](https://github.com/hurricanepkt/SmartHomeCallBacker)


## Contributing

Pull requests are welcome. For major changes, please open an issue first
to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License

[MIT](https://github.com/hurricanepkt/SmartHomeCallBacker/blob/main/LICENSE)