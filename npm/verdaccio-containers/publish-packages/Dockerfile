FROM node:14
ARG next_version
ENV next_version=$next_version
RUN mkdir /publish
COPY . /publish
WORKDIR /publish
EXPOSE 4872/tcp

RUN apt-get update
RUN apt-get install nano
RUN chmod +x ./entrypoint.sh
CMD /bin/bash ./entrypoint.sh