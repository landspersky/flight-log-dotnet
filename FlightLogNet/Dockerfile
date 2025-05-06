FROM mcr.microsoft.com/dotnet/runtime-deps:9.0-alpine
WORKDIR /app

RUN apk update && apk add git bash

RUN addgroup -S appgroup && \
	adduser -S --shell /bin/bash --ingroup appgroup appuser && \
	mkdir -p /data && \
	chown appuser:appgroup /data && \
	chmod 755 /data

COPY ./bin/Release/net9.0/linux-musl-x64/publish ./

USER appuser
ENTRYPOINT ["./FlightLogNet"]
