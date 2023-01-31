import ReactDOM from 'react-dom/client';
import 'semantic-ui-css/semantic.min.css';
import 'react-calendar/dist/Calendar.css';
import 'react-toastify/dist/ReactToastify.min.css';
import 'react-datepicker/dist/react-datepicker.css';
import './app/layout/styles.css';
import reportWebVitals from './reportWebVitals';
import { store, StoreContext } from './app/stores/store';
import { RouterProvider } from 'react-router-dom';
import { router } from './app/router/Routes';

// Will get the root element from index.html and render the app there.
const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);

// <App /> puts in root the content in App.tsx (function App())
// Using strict mode will render twice
// Provide context to application
root.render(
  <StoreContext.Provider value={store}>
    <RouterProvider router = {router} />
  </StoreContext.Provider>);

/*
ReactDOM.render(
  <StoreContext.Provider value={store}>
    <App />
  </StoreContext.Provider>,
  document.getElementById("root")
);*/

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
