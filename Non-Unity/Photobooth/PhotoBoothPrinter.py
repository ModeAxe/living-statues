import time
import os
import win32print
import win32ui
import win32con
from PIL import Image, ImageWin
import PIL
from watchdog.observers import Observer
from watchdog.events import FileSystemEventHandler

class ImageHandler(FileSystemEventHandler):
    def __init__(self, folder_to_watch):
        self.folder_to_watch = folder_to_watch

    def on_created(self, event):
        if event.is_directory:
            return
        _, extension = os.path.splitext(event.src_path)
        if extension.lower() in ['.png', '.jpg', '.jpeg', '.gif', '.bmp', '.tiff']:
            print(f"New image found: {event.src_path}")
            time.sleep(3)
            self.print_image(event.src_path)

    def print_image(self, file_path):
        img = Image.open(file_path)
        img = img.rotate(90, PIL.Image.NEAREST, expand=1)

        # Convert size from mm to inches for the paper size (100 x 148 mm to 3.937 x 5.827 inches)
        paper_size = (3.937, 5.827)
        printer = win32print.GetDefaultPrinter()
        hprinter = win32print.OpenPrinter(printer)
        hdc = win32ui.CreateDC()
        hdc.CreatePrinterDC(printer)

        # Get printer resolution
        horz_res = hdc.GetDeviceCaps(win32con.HORZRES)
        vert_res = hdc.GetDeviceCaps(win32con.VERTRES)
        horz_size = hdc.GetDeviceCaps(win32con.HORZSIZE) / 25.4  # mm to inches
        vert_size = hdc.GetDeviceCaps(win32con.VERTSIZE) / 25.4  # mm to inches

        scale_x = horz_res / horz_size
        scale_y = vert_res / vert_size

        paper_width = int(paper_size[0] * scale_x)
        paper_height = int(paper_size[1] * scale_y)

        hdc.StartDoc(file_path)
        hdc.StartPage()
        dib = ImageWin.Dib(img)

        # Center the image on the page
        x0 = (horz_res - paper_width) // 2
        y0 = (vert_res - paper_height) // 2
        x1 = x0 + paper_width
        y1 = y0 + paper_height

        dib.draw(hdc.GetHandleOutput(), (x0, y0, x1, y1))
        hdc.EndPage()
        hdc.EndDoc()
        hdc.DeleteDC()
        win32print.ClosePrinter(hprinter)

if __name__ == "__main__":
    folder_to_watch = "EDIT-PHOTO-LOCATION"
    event_handler = ImageHandler(folder_to_watch)
    observer = Observer()
    observer.schedule(event_handler, path=folder_to_watch, recursive=False)

    observer.start()
    print(f"Monitoring folder: {folder_to_watch}")

    try:
        while True:
            time.sleep(1)
    except KeyboardInterrupt:
        observer.stop()
    observer.join()
