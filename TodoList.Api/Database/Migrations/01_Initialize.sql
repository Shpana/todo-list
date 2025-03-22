CREATE TABLE IF NOT EXISTS users (
    id SERIAL PRIMARY KEY,
    name VARCHAR(64) NOT NULL,
    email VARCHAR(64) UNIQUE NOT NULL,
    hashed_password VARCHAR(84) NOT NULL,
    created_at TIMESTAMP NOT NULL,
    updated_at TIMESTAMP NOT NULL
);

CREATE INDEX IF NOT EXISTS users_email_index ON users(email);

CREATE TABLE IF NOT EXISTS user_login_sessions (
    id INTEGER REFERENCES users(id) NOT NULL,
    auth_token VARCHAR(64) NOT NULL,
    created_at TIMESTAMP NOT NULL,
    updated_at TIMESTAMP NOT NULL,
    PRIMARY KEY (id)
);