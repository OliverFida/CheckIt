#!/bin/bash

if [ -z "$CHECKIT_BACKEND_PROTOCOL" ] 
then
	if [ -f "/www/backend_config.json" ]
	then
		rm /www/backend_config.json
	fi
else
	echo "{\"PROTOCOL\": $CHECKIT_BACKEND_PROTOCOL, \"ADDRESS\": $CHECKIT_BACKEND_ADDRESS, \"PORT\": $CHECKIT_BACKEND_PORT}" > /www/backend_config.json
fi