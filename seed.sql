INSERT INTO equipment (inventory_number, name, department) VALUES
('INV-001', 'Сервер Dell R740',    'IT'),
('INV-002', 'Принтер HP LaserJet', 'Бухгалтерия');

INSERT INTO employees (full_name, position, department) VALUES
('Иванов Иван Иванович',    'Инженер',   'IT'),
('Петров Петр Петрович',    'Бухгалтер', 'Бухгалтерия'),
('Сидорова Анна Сергеевна', 'Менеджер',  'Продажи');

INSERT INTO tickets
    (author_id, equipment_id, assignee_id, title, description, priority, status, created_at)
VALUES
(1, 1, 1,    'Не запускается сервер', 'При включении нет реакции', 'Critical', 'New',        now() - interval '5 hours'),
(2, 2, 2,    'Замятие бумаги',        'Принтер зажевывает листы',  'Normal',   'InProgress', now() - interval '2 hours'),
(3, 1, 1,    'Медленно работает',     'Долгая загрузка страниц', 'High',     'Resolved',   now() - interval '10 hours'),
(1, 2, NULL, 'Заменить картридж',     'Закончился тонер',          'Low',      'Closed',     now() - interval '3 days'),
(2, 1, NULL, 'Отмена заявки',         'Ошибочно создана',          'Low',      'Cancelled',  now() - interval '1 day');

INSERT INTO ticket_status_history (ticket_id, from_status, to_status, changed_at, changed_by_id) VALUES
(2, 'New',        'InProgress', now() - interval '1 hour',  1),
(3, 'New',        'InProgress', now() - interval '9 hours', 1),
(3, 'InProgress', 'Resolved',   now() - interval '8 hours', 1),
(4, 'New',        'InProgress', now() - interval '2 days',  2),
(4, 'InProgress', 'Resolved',   now() - interval '2 days',  2),
(4, 'Resolved',   'Closed',     now() - interval '2 days',  2),
(5, 'New',        'Cancelled',  now() - interval '1 day',   2);

INSERT INTO comments (ticket_id, author_id, text) VALUES
(2, 2, 'Проблема повторяется раз в день');