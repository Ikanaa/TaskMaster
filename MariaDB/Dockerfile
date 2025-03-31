FROM mariadb:11-ubi

# Copy the startup SQL script
COPY startup.sql /docker-entrypoint-initdb.d/

# Set environment variable for MariaDB root password
ENV MARIADB_ROOT_PASSWORD="s"



EXPOSE 3306