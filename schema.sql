CREATE TABLE equipment (
    id               SERIAL PRIMARY KEY,
    inventory_number VARCHAR(50)  NOT NULL UNIQUE,
    name             VARCHAR(200) NOT NULL,
    department       VARCHAR(100) NOT NULL,
    is_deleted       BOOLEAN      NOT NULL DEFAULT FALSE
);

CREATE TABLE employees (
    id          SERIAL PRIMARY KEY,
    full_name   VARCHAR(200) NOT NULL,
    position    VARCHAR(100) NOT NULL,
    department  VARCHAR(100) NOT NULL,
    is_deleted  BOOLEAN      NOT NULL DEFAULT FALSE
);

CREATE TABLE tickets (
    id           SERIAL PRIMARY KEY,
    author_id    INTEGER NOT NULL REFERENCES employees(id),
    equipment_id INTEGER NOT NULL REFERENCES equipment(id),
    assignee_id  INTEGER REFERENCES employees(id),
    title        VARCHAR(200) NOT NULL CHECK (title <> ''),
    description  TEXT NOT NULL CHECK (length(description) <= 2000),
    priority     VARCHAR(20) NOT NULL
                 CHECK (priority IN ('Low','Normal','High','Critical')),
    status       VARCHAR(20) NOT NULL DEFAULT 'New'
                 CHECK (status IN ('New','InProgress','Resolved','Closed','Cancelled')),
    created_at   TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE INDEX ix_tickets_status      ON tickets(status);
CREATE INDEX ix_tickets_priority    ON tickets(priority);
CREATE INDEX ix_tickets_created_at  ON tickets(created_at);
CREATE INDEX ix_tickets_assignee_id ON tickets(assignee_id);

CREATE TABLE ticket_status_history (
    id            SERIAL PRIMARY KEY,
    ticket_id     INTEGER NOT NULL REFERENCES tickets(id) ON DELETE CASCADE,
    from_status   VARCHAR(20),
    to_status     VARCHAR(20) NOT NULL
                  CHECK (to_status IN ('New','InProgress','Resolved','Closed','Cancelled')),
    changed_at    TIMESTAMPTZ NOT NULL DEFAULT now(),
    changed_by_id INTEGER REFERENCES employees(id)
);
CREATE INDEX ix_history_ticket_id ON ticket_status_history(ticket_id);

CREATE TABLE comments (
    id         SERIAL PRIMARY KEY,
    ticket_id  INTEGER NOT NULL REFERENCES tickets(id) ON DELETE CASCADE,
    author_id  INTEGER NOT NULL REFERENCES employees(id),
    text       TEXT NOT NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now()
);
CREATE INDEX ix_comments_ticket_id ON comments(ticket_id);