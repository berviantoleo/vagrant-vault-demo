#!/bin/sh -ex

IP_ADDRESS=192.168.56.4
PORT=30008
RETRY_TIMES=15
echo
echo "Testing Root"
curl --retry $RETRY_TIMES --retry-connrefused "http://$IP_ADDRESS:$PORT/" -v
echo
echo "Testing Add Secret"
curl --retry $RETRY_TIMES --retry-connrefused -X 'POST' -d '{"Name": "test", "Data": {"test": "ok"}}' -H 'Content-Type: application/json' "http://$IP_ADDRESS:$PORT/secrets" -v
echo
echo "Testing Retrieve Secret"
curl --retry $RETRY_TIMES --retry-connrefused "http://$IP_ADDRESS:$PORT/secrets/test" -v
echo
