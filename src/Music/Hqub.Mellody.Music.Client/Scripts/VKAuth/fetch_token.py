__author__ = 'Boris'

import vk_auth
import sys

if __name__ == '__main__' and len(sys.argv) > 4:
    client_id = sys.argv[1]   # Vk application ID
    email = sys.argv[2]
    password = sys.argv[3]
    scope = sys.argv[4]

    token, user_id = vk_auth.auth(email, password, client_id, scope)

    print token