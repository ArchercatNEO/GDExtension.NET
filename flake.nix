{
  description = "Development environment for GodotSharp";

  inputs = {
    nixpkgs.url = "github:nixos/nixpkgs?ref=nixos-unstable";
  };

  outputs = { self, nixpkgs }: 
  let
    system = "x86_64-linux";
    pkgs = import nixpkgs {
      inherit system;
    };
  in
  {
    devShells."${system}".default = pkgs.mkShell {
      packages = with pkgs; [
        git
        gdb
        dotnet-sdk_8
      ];
    };
  };
}
