#!/bin/sh -ex

IP_ADDRESS=192.168.56.4
PORT=30008
echo
echo "Testing Root"
curl --retry 5 --retry-connrefused "http://$IP_ADDRESS:$PORT/" -v
echo
echo "Testing Add Secret"
curl --retry 5 --retry-connrefused -X 'POST' -d '{"Name": "test", "Data": {"test": "ok"}}' -H 'Content-Type: application/json' "http://$IP_ADDRESS:$PORT/secrets" -v
echo
echo "Testing Retrieve Secret"
curl --retry 5 --retry-connrefused "http://$IP_ADDRESS:$PORT/secrets/test" -v
echo
