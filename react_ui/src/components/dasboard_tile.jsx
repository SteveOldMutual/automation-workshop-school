import './styles/tileStyles.css'

const DashboardTile = ({tileTitle, tileValue}) => {
    
    return <div className="tile" title={tileTitle}>
    <div className="tile-body">
        <h5 className="tile-title">{tileTitle}</h5>
        <p role="tileValue" title="tileValue" className="tile-text">{tileValue}</p>
        
    </div>
</div>
};

export default DashboardTile;

