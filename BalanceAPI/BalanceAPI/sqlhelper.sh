#!/bin/bash
# wait-for-it.sh

host="$1"
shift
cmd="$@"

until nc -z "$host" 1433; do
  echo "Waiting for SQL Server..."
  sleep 5
done

exec $cmd