set -e

export PGPASSWORD="${POSTGRES_PASSWORD}"

find /migrations -maxdepth 2 -name "*.sql" -print -exec psql \
   --host "database" \
   --username "${POSTGRES_USER}" \
   --dbname "${POSTGRES_DB}" \
   -f {} \;