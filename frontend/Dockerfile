
FROM node:14

WORKDIR /app

COPY ./frontend/package.json .
COPY ./frontend/package-lock.json .

RUN npm install -g @angular/cli
RUN npm install
