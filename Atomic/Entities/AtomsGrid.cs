﻿using PureFreak.TileMore;
using System;

namespace Atomic.Entities
{
    public class AtomsGrid
    {
        private readonly Contents _contents;
        private readonly int _tileSize;
        private readonly int _tilesWidth;
        private readonly int _tilesHeight;
        private readonly GridAtom[,] _atoms;

        public AtomsGrid(Contents contents, int tileSize, int tilesWidth, int tilesHeight)
        {
            _contents = contents;
            _tileSize = tileSize;
            _tilesWidth = tilesWidth;
            _tilesHeight = tilesHeight;
            _atoms = new GridAtom[tilesWidth, tilesHeight];
        }

        public Atom CreateAtom(int? electrons = null)
        {
            if (electrons.HasValue && (electrons < 0 || electrons > 4))
                throw new ArgumentException("Electrons must be a value between 1 and 4");

            if (!electrons.HasValue)
                electrons = RandomHelper.Between(new Range<int>(1, 4));

            return new Atom(_contents, electrons.Value);
        }

        public bool SetAtom(int gridX, int gridY, Atom atom)
        {
            if (atom == null)
                throw new ArgumentNullException(nameof(atom));

            if (IsValidPos(gridX, gridY) && !HasAtom(gridX, gridY))
            {
                _atoms[gridX, gridY] = new GridAtom(this, gridX, gridY, atom.Electrons);

                AtomAdded(_atoms[gridX, gridY]);

                return true;
            }

            return false;
        }

        private void AtomAdded(GridAtom addedAtom)
        {
            for (int gridX = addedAtom.GridX - 1; gridX < addedAtom.GridX + 1; gridX++)
            {
                for (int gridY = addedAtom.GridY - 1; gridY < addedAtom.GridY + 1; gridY++)
                {
                    var atom = GetAtom(gridX, gridY);
                    if (atom != null) atom.RefreshNeighbours();
                }
            }
        }

        public GridAtom GetAtom(int gridX, int gridY)
        {
            if (IsValidPos(gridX, gridY))
                return _atoms[gridX, gridY];

            return null;
        }

        public bool HasAtom(int gridX, int gridY)
        {
            return GetAtom(gridX, gridY) != null;
        }

        public bool IsValidPos(int gridX, int gridY)
        {
            return
                gridX >= 0 &&
                gridY >= 0 &&
                gridX < _tilesWidth &&
                gridY < _tilesHeight;
        }

        public int TileSize
        {
            get { return _tileSize; }
        }

        public GridAtom[,] Atoms
        {
            get { return _atoms; }
        }

        public int TilesWidth
        {
            get { return _tilesWidth; }
        }

        public int TilesHeight
        {
            get { return _tilesHeight; }
        }

        public int Width
        {
            get { return _tilesWidth * _tileSize; }
        }

        public int Height
        {
            get { return _tilesHeight * _tileSize; }
        }

        public Contents Contents
        {
            get { return _contents; }
        }
    }
}