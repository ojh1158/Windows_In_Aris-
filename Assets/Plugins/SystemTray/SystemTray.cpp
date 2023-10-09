#include <Windows.h>
#include <iostream>

#define WM_TRAYICON (WM_USER + 1)

HINSTANCE hInstance;
NOTIFYICONDATA nid;

LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam) {
    if (message == WM_TRAYICON) {
        if (lParam == WM_RBUTTONDOWN) {
            POINT pt;
            GetCursorPos(&pt);

            HMENU hMenu = CreatePopupMenu();
            AppendMenu(hMenu, MF_STRING, 1001, "Reset");
            AppendMenu(hMenu, MF_STRING, 1002, "Exit");

            SetForegroundWindow(hWnd);
            TrackPopupMenu(hMenu, TPM_BOTTOMALIGN, pt.x, pt.y, 0, hWnd, NULL);
            PostMessage(hWnd, WM_NULL, 0, 0);

            DestroyMenu(hMenu);
        }
        return 1;
    }

    switch (message) {
        case WM_COMMAND:
            switch (LOWORD(wParam)) {
                case 1001: // Option 1
                    // Option 1 코드
                    break;
                case 1002: // Exit
                    // 종료 코드
                    break;
            }
            break;
    }
    return DefWindowProc(hWnd, message, wParam, lParam);
}

BOOL APIENTRY DllMain(HINSTANCE hInst, DWORD reason, LPVOID reserved) {
    switch (reason) {
    case DLL_PROCESS_ATTACH:
        hInstance = hInst;
        nid.cbSize = sizeof(nid);
        nid.hWnd = NULL;
        nid.uFlags = NIF_ICON | NIF_MESSAGE | NIF_TIP;
        nid.uCallbackMessage = WM_USER;
        nid.hIcon = LoadIcon(hInstance, MAKEINTRESOURCE(101)); // 101 is ID of your icon
        strcpy(nid.szTip, "Aris");
        break;

    case DLL_PROCESS_DETACH:
        Shell_NotifyIcon(NIM_DELETE, &nid);
        break;
    }
    return TRUE;
}

typedef void(__stdcall *ResetCallBack)(const char* message);

ResetCallBack resetCallBack;

extern "C" {
    __declspec(dllexport) void ShowTrayIcon() {
        Shell_NotifyIcon(NIM_ADD, &nid);
    }

    __declspec(dllexport) void HideTrayIcon() {
        Shell_NotifyIcon(NIM_DELETE, &nid);
    }

    __declspec(dllexport) void RegisterCallback(ResetCallBack callback)
    {
        resetCallBack = callback;

        if (resetCallBack)
            resetCallBack("Callback registered and called from C++!");
    }
}
