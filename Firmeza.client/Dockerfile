# -------------------------------------
# 1) Build — Node 20
# -------------------------------------
FROM node:20 AS build
WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . .
RUN npm run build

# -------------------------------------
# 2) Deploy — NGINX
# -------------------------------------
FROM nginx:stable-alpine

# Copia archivos de build
COPY --from=build /app/dist /usr/share/nginx/html

# Copia configuración personalizada
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
