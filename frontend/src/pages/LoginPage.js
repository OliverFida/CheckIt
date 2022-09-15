import { Button, Form, Input, Col, Row } from 'antd';
import React from 'react';

export default function LoginPage(){
    
    const onFinish = (values) => {
        console.log('Success:', values);
      };
    
      const onFinishFailed = (errorInfo) => {
        console.log('Failed:', errorInfo);
      };
        
      
        return (
        <Row type="flex" justify="center" align="center">
            <Col span={12} >
                <Form
                    name="basic"
                    labelCol={{
                    span: 8,
                    }}
                    wrapperCol={{
                    span: 8,
                    }}
                    initialValues={{
                    remember: true,
                    }}
                    onFinish={onFinish}
                    onFinishFailed={onFinishFailed}
                    autoComplete="off"
                >
                    <Form.Item
                    label="Benutzername"
                    name="username"
                    rules={[
                        {
                        required: true,
                        message: 'Bitte geben Sie Ihren Benutzernamen ein!',
                        },
                    ]}
                    >
                    <Input />
                    </Form.Item>
            
                    <Form.Item
                    label="Passwort"
                    name="password"
                    rules={[
                        {
                        required: true,
                        message: 'Bitte geben Sie Ihr Passwort ein!',
                        },
                    ]}
                    >
                    <Input.Password />
                    </Form.Item> 
                
            
                    <Form.Item
                    wrapperCol={{
                        offset: 8,
                        span: 16,
                    }}
                    >
                    <Button type="primary" htmlType="submit">
                        Einloggen
                    </Button>
                    </Form.Item>
                </Form>
            </Col>
        </Row>
    );
};

      