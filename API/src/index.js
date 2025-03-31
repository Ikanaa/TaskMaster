const express = require('express');
const cors = require('cors');
const mariadb = require('mariadb');

// Initialize the app
const app = express();
const PORT = process.env.PORT || 3000;

// MariaDB Connection
// Determine how to connect to the database
const dbConfig = {
    host: process.env.DB_HOST || 'localhost',
    user: process.env.DB_USER || 'root',
    password: process.env.DB_PASSWORD || '',
    //database: process.env.DB_NAME || 'taskmanager',
    connectionLimit: 5
};

// Create connection pool
const pool = mariadb.createPool(dbConfig);

// Test database connection
async function testConnection() {
    console.log('Testing database connection...');
    console.log(dbConfig);
    let conn;
    try {
        conn = await pool.getConnection();
        console.log('Connected to MariaDB successfully!');
    } catch (err) {
        console.error('Database connection error:', err);
    } finally {
        if (conn) conn.release();
    }
}

// Test connection on startup
testConnection();

// Export pool for use in other modules
module.exports = { pool };

// Middleware
app.use(cors());
app.use(express.json());
app.use(express.urlencoded({ extended: true }));

// Routes
app.get('/', (req, res) => {
    res.send('Task Manager API is running');
});


// Error handler
app.use((err, req, res, next) => {
    console.error(err.stack);
    res.status(500).json({ message: 'Something went wrong!' });
});

// Start server
app.listen(PORT, () => {
    console.log(`Server running on port ${PORT} http://localhost:${PORT}`);
});