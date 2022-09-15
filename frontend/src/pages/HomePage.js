// Component imports
import {Layout, Space, Button} from 'antd';
// Style imports
import '../css/pages/HomePage.css';

const {Header, Content} = Layout;

export default function HomePage(){
    return(
        <Layout>
            <AppHeader />
            <Content>
                
            </Content>
        </Layout>
    );
};

function AppHeader(){
    return(
        <Header id="Header" theme='light'>
            <Space direction='horizontal'>
                <h1>CHECK-IT</h1>
                <Button type='ghost'>Abmelden</Button>
            </Space>
        </Header>
    );
}