# LocoSQS

A local AWS SQS implementation meant for local and automated testing.

## Usage

Build and run the docker container. Default port is 8080.

To build the image, run

``` sh
docker build . --no-cache --tag locosqs:latest
```

To run the image without persistent storage, run

```sh
docker run -p 8080:8080 locosqs:latest
```

If persistent storage is preferred, create a file called `data.json` in the current working directory, write `[]` to it, then run

```sh
docker run --env JSON="./data.json" -v ./data.json:/app/data.json:Z -p 8080:8080 locosqs:latest
```

If a different port is preferred, run

```sh
docker run --env PORT=5678 -p 5678:5678 locosqs:latest
```

### Usage inside code

The official AWS SDK's can be used with LocoSQS. To do this, configure the client as follows:
```
Service Url: 'http://localhost:8080'
Access Key: 'x'
Secret Key: 'x'
```

### Usage of web interface

Navigate to `http://localhost:8080` in a web browser to access the web interface

## Configuration
LocoSQS is configured trough environment variables. The following variables are available:

| Environment Variable  | Default Value | Description |
| - | - | - |
| PORT | 8080 | Port to run the web server on. Affects returned queue url's. If changing this, don't forget to change your docker command accordingly. **The port accessible outside has to match the internal port**. |
| HOST | localhost | Only relevant if running outside of your local machine. Affects returned queue url's. |
| PROTOCOL | http | Affects returned queue url's. |
| JSON | (null) | Set this to a path if persistent storage in the form of JSON is preferred. |
| READONLY | (null) | Set this to `1` if the storage backend should not perform any writes. Used for example to have a read-only JSON configuration. |
| NOTRACK | (null) | Set this to `1` to stop tracking messages for the webhook. Note: This does break webhook functionality. |
| DEBUG | (null) | Set this to `1` to log message updates to the console. |
| CREATEQUEUEIMPLICITLY | (null) | Set this to `1` to create a queue automatically when you try to access a non-existant queue. |