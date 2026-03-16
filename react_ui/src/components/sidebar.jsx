import React from 'react';
import { Link } from 'react-router-dom';

const Sidebar = () => {
    return (
        <nav style={styles.nav}>
            <ul style={styles.ul}>
                <li style={styles.li}>
                    <Link to="/" style={styles.link}>Home</Link>
                </li>
                <li style={styles.li}>
                    <Link to="/students" style={styles.link}>Students</Link>
                </li>
                <li style={styles.li}>
                    <Link to="/rollcall" style={styles.link}>Rollcall</Link>
                </li>
                <li style={styles.li}>
                    <Link to="/dismissals" style={styles.link}>Dismissals</Link>
                </li>
               
            </ul>
        </nav>
    );
};

const styles = {
    nav: {
        width: '250px',
        background: '#343a40',
        padding: '10px',
        height: '100vh',
        position: 'fixed',
        zIndex: 1000,
        color: '#fff',
    },
    ul: {
        listStyleType: 'none',
        padding: 0,
    },
    li: {
        marginBottom: '15px',
    },
    link: {
        textDecoration: 'none',
        color: '#adb5bd',
        fontSize: '16px',
        fontWeight: '500',
    },
    linkActive: {
        color: '#fff',
        fontWeight: '700',
    },
};

export default Sidebar;