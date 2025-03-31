FROM node:23.10.0

# Set working directory
WORKDIR /app

# Copy package files first for better caching
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy source code
COPY src/ ./src/

# Expose port (uncomment and change if needed)
EXPOSE 3000

# Command to run the application
CMD ["node", "src/index.js"]
