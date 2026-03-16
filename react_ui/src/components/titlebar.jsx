import './styles/barStyle.css'

const TitleBar = ({pageTitle}) => {
    const handleLogout = () => {
        localStorage.removeItem('token');
        window.location.href = '/';
    };

    return (
        <div className="dashboard-header" style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
            <h1 style={{ margin: 0 }}>{pageTitle}</h1>
            <button onClick={handleLogout} style={{ marginLeft: 'auto' }}>Logout</button>
        </div>
    );
};

export default TitleBar;

