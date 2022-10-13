echo "{" > /www/backend_config.json
echo "$CHECKIT_BACKEND_PROTOCOL" >> /www/backend_config.json
echo "," > /www/backend_config.json
echo "$CHECKIT_BACKEND_ADDRESS" >> /www/backend_config.json
echo "," > /www/backend_config.json
echo "$CHECKIT_BACKEND_PORT" >> /www/backend_config.json
echo "}" > /www/backend_config.json
