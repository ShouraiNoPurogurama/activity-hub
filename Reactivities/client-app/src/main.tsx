import React from 'react'
import ReactDOM from 'react-dom/client'
import 'react-calendar/dist/Calendar.css'
import 'semantic-ui-css/semantic.min.css'
import './app/layout/styles.css'
import 'react-toastify/dist/ReactToastify.min.css'
import { store, StoreContext } from './app/stores/store.ts'
import { RouterProvider } from 'react-router-dom'
import { router } from './app/router/Route.tsx'

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <StoreContext.Provider value={store}>
      <RouterProvider router={router}></RouterProvider>
    </StoreContext.Provider>
  </React.StrictMode>,
)
