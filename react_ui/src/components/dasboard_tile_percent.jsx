import './styles/tileStyles.css'
import 'bootstrap/dist/css/bootstrap.min.css';

const DashboardTilePercent =({ tileTitle, tileValue , tileTotal}) => {

    return <div className="tile" title={tileTitle}>
        <div className="tile-body">
            <h5 className="tile-title">{tileTitle}</h5>
            <p title="tileValue" className="tile-text">{tileValue} / {tileTotal}</p>
            <div className="progress">
                <div 
                    title="tileProgress" 
                    className="progress-bar" 
                    role="progressbar" 
                    aria-valuenow={(tileValue/tileTotal)*100} 
                    aria-valuemin="0" 
                    aria-valuemax="100" 
                    style={{ width: `${(tileValue / tileTotal) * 100}%` }}>
                </div>
            </div>
        </div>
    </div>
};

export default DashboardTilePercent;

