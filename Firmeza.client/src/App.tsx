import { Outlet } from 'react-router-dom';

function App() {
    return (
        <div className="w-full bg-gray-50 flex justify-center items-center">
            <Outlet />
        </div>
    );
}

export default App;
