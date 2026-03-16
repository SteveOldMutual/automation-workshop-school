import React from 'react';

const DataTable = ({ data, hideColumns, actionButtons }) => {
    const [currentPage, setCurrentPage] = React.useState(1);
    const [searchTerm, setSearchTerm] = React.useState("");

    if (!data || data.length === 0) {
        return <p>No data available</p>;
    }

    // Get column headers from the keys of the first object
    let columns = Object.keys(data[0]);
    // Filter out the columns to hide
    if (hideColumns && hideColumns.length > 0) {
        columns = columns.filter((column) => !hideColumns.map(col => col.toUpperCase()).includes(column.toUpperCase()));
    }

    const rowsPerPage = 10;

    // Calculate the indices for the current page
    const indexOfLastRow = currentPage * rowsPerPage;
    const indexOfFirstRow = indexOfLastRow - rowsPerPage;
    const currentRows = data.slice(indexOfFirstRow, indexOfLastRow);

    const totalPages = Math.ceil(data.length / rowsPerPage);

    const handlePageChange = (pageNumber) => {
        setCurrentPage(pageNumber);
    };


    const handleSearch = (event) => {
        setSearchTerm(event.target.value.toLowerCase());
        setCurrentPage(1); // Reset to the first page when searching
    };

    const filteredRows = data.filter((row) =>
        columns.some((column) =>
            String(row[column]).toLowerCase().includes(searchTerm)
        )
    );

    const totalFilteredPages = Math.ceil(filteredRows.length / rowsPerPage);
    const filteredCurrentRows = filteredRows.slice(indexOfFirstRow, indexOfLastRow);

    return (
        <div>
            <div className="search-bar" style={{ marginBottom: '10px' }}>
                <input
                    type="text"
                    placeholder="Search..."
                    value={searchTerm}
                    onChange={handleSearch}
                    className="form-control"
                />
            </div>
            <table className='table table-striped table-bordered'>
                <thead className='thead-dark'>
                    <tr>
                        {columns.map((column) => (
                            <th key={column}>{column}</th>
                        ))}
                        {actionButtons && actionButtons.length > 0 && <th>Actions</th>}
                    </tr>
                </thead>
                <tbody>
                    {filteredCurrentRows.map((row, rowIndex) => (
                        <tr key={rowIndex}>
                            {columns.map((column) => (
                                <td key={column}>{row[column]}</td>
                            ))}
                            {actionButtons && actionButtons.length > 0 && (
                                <td>
                                    {actionButtons.map(({ label, onClick, style }, index) => (
                                        <button
                                            key={index}
                                            onClick={() => onClick(row)}
                                            className="btn btn-primary btn-sm"
                                            style={{ marginRight: '5px', ...style }}
                                        >
                                            {label}
                                        </button>
                                    ))}
                                </td>
                            )}
                        </tr>
                    ))}
                </tbody>
            </table>
            <div className="pagination">
                {Array.from({ length: totalFilteredPages }, (_, index) => (
                    <button
                        key={index}
                        onClick={() => handlePageChange(index + 1)}
                        className={`btn ${currentPage === index + 1 ? 'btn-primary' : 'btn-secondary'} btn-sm`}
                        style={{ marginRight: '5px' }}
                    >
                        {index + 1}
                    </button>
                ))}
            </div>
        </div>
    );
};

export default DataTable;
