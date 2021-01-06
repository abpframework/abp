FROM node:14
RUN mkdir /app
COPY . /app
WORKDIR /app
EXPOSE 4200/tcp

RUN apt-get update
RUN apt-get install nano
RUN chmod +x ./entrypoint.sh
CMD /bin/bash ./entrypoint.sh