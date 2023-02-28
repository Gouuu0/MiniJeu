import random
import socketserver
import string
import threading
import time

class Player:
    def __init__(self, id, x, y, r):
        self.id = id
        self.x = x
        self.y = y 
        self.r = r

client_sockets = []
players = []

def removeClientSocket(socket):
    if socket in client_sockets:
        client_sockets.remove(socket)

def removePlayer(player):
    if player in players:
        players.remove(player)

def send_message_to_clients(message):
    for client in client_sockets:
        try:
            client.sendall(message.encode())
        except:
            print('Client disconnected')
            removeClientSocket(client)

class MyTCPHandler(socketserver.BaseRequestHandler):
    def handle(self):
        # self.request is the TCP socket connected to the client
        address = self.client_address
        client_sockets.append(self.request)
        player = None
        try:
            while True:
                self.data = self.request.recv(4096).strip()
                if self.data:
                    print("{} wrote:".format(address[0]))
                    print(self.data)
                    data = self.data.decode().split("/")
                    for i in range(len(data)-1):

                        playerargs = data[i].split(":")

                        if playerargs[0] == "JOIN":
                            player = Player(random.randint(0,2147483647),0.0,0.0,0.0)
                            players.append(player)
                            response :str = "CONNECTED:"+str(player.id)
                            self.request.sendall(response.encode())
                        elif playerargs[0] == "DISCONNECT":
                            response :str = "PLAYERDELETE:"+str(player.id)
                            send_message_to_clients(response)
                            print('Player disconnected')
                            removePlayer(player)
                            removeClientSocket(self.request)
                            break
                        elif playerargs[0] == "POS":
                            player.x = float(playerargs[1].replace(",","."))
                            player.y = float(playerargs[2].replace(",","."))
                            player.r = float(playerargs[3].replace(",","."))
                        else:
                            self.request.sendall(b'Error')

        except Exception as e:
            print("Error Disconnecting from server")
            print(e)
            removePlayer(player)
            removeClientSocket(self.request)

if __name__ == "__main__":
    HOST, PORT = "0.0.0.0", 6700

    # Create the server, binding to localhost on port 6700
    with socketserver.ThreadingTCPServer((HOST, PORT), MyTCPHandler) as server:

        def sendPositionToClient():
            while True:
                message :str = "PLAYERS:"
                for player in players:
                    message += str(player.id) +"|"+str(player.x).replace(".",",")+"|"+str(player.y).replace(".",",") + "|" + str(player.r).replace(".",",") + ":"
                
                message = message[:-1]
                print(message)
                send_message_to_clients(message)
                time.sleep(0.05)

        thread = threading.Thread(target=sendPositionToClient)
        thread.start()

        server.serve_forever()