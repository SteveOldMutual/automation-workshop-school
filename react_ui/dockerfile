# Use a Node.js image to build the React app
FROM node:14 AS build

# Set the working directory in the container
WORKDIR /usr/src/app

# Copy the package.json and package-lock.json files
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the rest of the application code
COPY . .

# Declare build-time variable and make it available to the build
ARG REACT_APP_API_URL
ENV REACT_APP_API_URL=$REACT_APP_API_URL

# Build the React app
RUN npm run build

# Install serve globally
RUN npm install -g serve

# Expose the port serve will run on
EXPOSE 3000

# Use serve to serve the built React app
CMD ["serve", "-s", "build"]