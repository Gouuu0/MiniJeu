import random
import socketserver
import string
import threading
import time

client_sockets = []

def randomword(length):
   letters = string.ascii_lowercase
   return ''.join(random.choice(letters) for i in range(length))

class MyTCPHandler(socketserver.BaseRequestHandler):
    def handle(self):
        # self.request is the TCP socket connected to the client
        address = self.client_address
        client_sockets.append(self.request)
        while True:
            self.data = self.request.recv(1024).strip()
            if self.data:
                print("{} wrote:".format(address[0]))
                print(self.data)

                if self.data == b'ButtonClicked':
                    self.request.sendall(b'HIDEBUTTON')
                elif self.data == b'player_disconnect':
                    print('Player disconnected')
                    client_sockets.remove(self.request)
                    break
                else:
                    self.request.sendall(b'Error')

if __name__ == "__main__":
    HOST, PORT = "0.0.0.0", 6700

    # Create the server, binding to localhost on port 6700
    with socketserver.ThreadingTCPServer((HOST, PORT), MyTCPHandler) as server:

        def send_message_to_clients():
            while True:
                for client in client_sockets:
                    try:
                        client.sendall(randomword(10).encode())
                    except:
                        print('Client disconnected')
                        client_sockets.remove(client)
                time.sleep(1)

        thread = threading.Thread(target=send_message_to_clients)
        thread.start()

        server.serve_forever()